import {useAuth} from "../../hooks/AuthProvider";
import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {getFlashcardSet, updateFlashcardSet} from "../../api/FlashcardSetApi";
import {randomId} from "@mui/x-data-grid-generator";
import PageTitle from "../../components/common/PageTitle";
import {Alert, Button, TextField} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Close";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid2";
import FlashCardCreate from "../../components/flashcardsets/FlashCardCreate";
import FlashCardEdit from "../../components/flashcardsets/FlashCardEdit";
import FlashCard from "../../components/flashcardsets/FlashCard";
import * as React from "react";

const FlashcardSetEdit = () => {
    let { setId } = useParams();
    const auth = useAuth();
    const navigate = useNavigate();

    const [flashcardSet, setFlashcardSet] = useState();
    const [displayAdd, setDisplayAdd] = useState(false);
    const [editCardId, setEditCardId] = useState();
    const [validName, setValidName] = useState(true);
    const [isValid, setIsValid] = useState(false);
    const [error, setError] = useState();

    const [isLoading, setIsLoading] = useState(true);

    async function fetchData() {
        if (auth.token) {
            const data = await getFlashcardSet(auth.token, setId);
            data.cards = data.cards.map((_) => {
                return {..._, id: randomId() };
            })
            setFlashcardSet(data);
            setIsLoading(false);
        }
    }

    useEffect(() => {
        fetchData();
        validateSet();
    }, [isLoading, setId]);

    function SaveFlashcardSet() {
        updateFlashcardSet(auth.token, flashcardSet).then((result) => {
            const setId = result.id;
            navigate(`/sets/${setId}`);
        }).catch((e) => {
            setError(e);
        });
    }

    function validateName() {
        if (isLoading) return false;
        setValidName(flashcardSet.name && flashcardSet.name.trim().length > 0);
        validateSet();
    }
    function validateSet() {
        if (isLoading) return false;
        setIsValid(flashcardSet.cards.length > 0 && flashcardSet.name && flashcardSet.name.trim().length > 0);
    }

    function addCard() {
        setDisplayAdd(true);
    }
    function AddCardCancel() {
        setDisplayAdd(false);
        validateSet();
    }
    function AddCardSave(data) {
        let newCards = flashcardSet.cards;
        newCards = [...newCards, {id: randomId(), question: data.question, answer: data.answer, difficulty: data.difficulty}];
        setFlashcardSet({...flashcardSet, cards: newCards});
        setDisplayAdd(false);
        validateSet();
    }

    function EditCard(card) {
        setEditCardId(card.id);
    }

    function EditCardCancel() {
        setEditCardId(undefined);
        validateSet();
    }

    function EditCardSave(card) {
        const newCards = flashcardSet.cards;
        const index =  newCards.findIndex(obj => obj.id === card.id);
        newCards[index] = card;
        setFlashcardSet({...flashcardSet, cards: newCards});
        setEditCardId(undefined);
        validateSet();
    }

    function DeleteCard(card) {
        const newCards = flashcardSet.cards.filter((obj) => obj.id !== card.id);
        setFlashcardSet({...flashcardSet, cards: newCards});
        validateSet();
    }

    return (!isLoading &&
        <>
            <PageTitle title={`Edit Flashcard Set`}>
                <Button
                    variant={"outlined"}
                    color={"info"}
                    startIcon={<AddIcon />}
                    title={"Add a flashcard"}
                    sx={{mr: 2}}
                    onClick={addCard}
                    disabled={displayAdd || editCardId}
                >
                    Add Card
                </Button>
                <Button
                    variant={"outlined"}
                    startIcon={<SaveIcon />}
                    title={"Create the flashcard set"}
                    sx={{mr: 2}}
                    disabled={!isValid}
                    onClick={SaveFlashcardSet}
                >
                    Save Flashcard Set
                </Button>
                <Link to={{pathname: `/users/${auth.userId}/sets`}}>
                    <Button
                        variant={"outlined"}
                        color={"secondary"}
                        startIcon={<CancelIcon />}
                        title={"Cancel"}
                    >
                        Cancel
                    </Button>
                </Link>
            </PageTitle>
            {
                error &&
                <Alert variant="outlined" severity="error" sx={{m: 2}}>
                    <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                        {error.message}
                    </Typography>
                </Alert>
            }
            <Grid container sx={{ display: 'flex', ml: 3, mr: 3, mt: 3, justifyContent: "center", alignContent: "top" }}>
                <Grid item size={{xs: 12, sm:6}}>
                    <Typography variant={"body1"} color={"primary"} sx={{mb: 1, pl: 1, fontWeight: "bold"}}>
                        Flashcard Set Name
                    </Typography>
                    <TextField
                        id="setName"
                        required
                        fullWidth
                        value={flashcardSet.name}
                        error={!validName}
                        helperText={validName ? '' : 'A question is required'}
                        placeholder={"Enter a flashcard set name"}
                        onChange={e => setFlashcardSet({ ...flashcardSet, name: e.target.value })}
                        onBlur={validateName}
                    />
                    <Typography variant={"body1"} color={"primary"} sx={{mb: 1, mt: 2, pl: 1, fontWeight: "bold"}}>
                        Owner
                    </Typography>
                    <TextField
                        id="setOwner"
                        disabled
                        fullWidth
                        variant={"filled"}
                        value={auth.username}
                    />
                </Grid>
            </Grid>
            <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 3, justifyContent: "center" }}>
                {
                    displayAdd &&
                    <Grid item size={12}>
                        <FlashCardCreate
                            addCardHandler={AddCardSave}
                            cancelAddHandler={AddCardCancel} />
                    </Grid>
                }
                {
                    (flashcardSet.cards.length === 0) &&
                    <Grid item size={12}>
                        <Alert variant="outlined" severity="info">
                            No flashcards to display
                        </Alert>
                    </Grid>
                }
                {
                    (flashcardSet.cards.length > 0) &&
                    flashcardSet.cards?.map((_, index) => {
                        return (
                            <Grid size={{ xs: 12, sm: 5, md: 3, lg: 4 }} key={_.id}>
                                {
                                    (editCardId === _.id) &&
                                    <FlashCardEdit
                                        target={_}
                                        editCardHandler={EditCardSave}
                                        cancelEditHandler={EditCardCancel}
                                    />
                                }
                                {
                                    (editCardId !== _.id) &&
                                    <FlashCard
                                        card={_}
                                        allowEdit={!displayAdd && !editCardId}
                                        defaultReveal={true}
                                        editCardHandler={EditCard}
                                        deleteCardHandler={DeleteCard}
                                    />
                                }
                            </Grid>
                        )
                    })
                }
            </Grid>
        </>
    );
}

export default FlashcardSetEdit;
