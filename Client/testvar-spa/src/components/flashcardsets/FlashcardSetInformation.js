import {Divider, Stack} from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import {formatTimestamp} from "../../utils/dateTime";
import * as React from "react";

const FlashcardSetInformation = ({set}) => {
    return (
        <>
            <Stack divider={<Divider orientation="horizontal" flexItem/>}>
                <Box sx={{display: "flex", justifyContent: "center", alignItems: "center"}}>
                    <Typography variant={"body2"} color={"secondary"} textAlign={"right"} sx={{p: 2, width: 1/2}}>Owner</Typography>
                    <Typography variant={"body1"} fontWeight={"bold"} sx={{p: 2, width: 1/2}}>{set.user.username}</Typography>
                </Box>
                <Box sx={{display: "flex", justifyContent: "center", alignItems: "center"}}>
                    <Typography variant={"body2"} color={"secondary"} textAlign={"right"} sx={{p: 2, width: 1/2}}>Created At</Typography>
                    <Typography variant={"body1"} fontWeight={"bold"} sx={{p: 2, width: 1/2}}>{formatTimestamp(set.created_at)}</Typography>
                </Box>
                <Box sx={{display: "flex", justifyContent: "center", alignItems: "center"}}>
                    <Typography variant={"body2"} color={"secondary"} textAlign={"right"} sx={{p: 2, width: 1/2}}>Updated At At</Typography>
                    <Typography variant={"body1"} fontWeight={"bold"} sx={{p: 2, width: 1/2}}>{formatTimestamp(set.updated_at)}</Typography>
                </Box>
            </Stack>
            <Divider orientation="horizontal" flexItem/>
        </>
    )
}
export default  FlashcardSetInformation