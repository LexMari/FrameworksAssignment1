const apiBaseUrl = "https://localhost:7222/api/sets";

export async function getFlashcardSets(token) {
    const options = {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    const response = await fetch(apiBaseUrl, options);
    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}

export async function getFlashcardSet(token, setId) {
    const url = `${apiBaseUrl}/${setId}`;
    console.log(url);

    const options = {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    const response = await fetch(url, options);
    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}

export async function createFlashcardSet(token, data) {
    const flashcardSet = {
        name: data.name,
        cards: data.cards.map((x) => {
            return {
                question: x.question,
                answer: x.answer,
                difficulty: x.difficulty
            }
        })
    }
    const options = {
        method: 'POST',
        body: JSON.stringify(flashcardSet),
        headers: {
            'Authorization': `Bearer ${token}`,
            "Content-type": "application/json; charset=UTF-8"
        }
    };

    const response = await fetch(apiBaseUrl, options);
    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}

export async function updateFlashcardSet(token, data) {
    const url = `${apiBaseUrl}/${data.id}`;
    const flashcardSet = {
        id: data.id,
        name: data.name,
        cards: data.cards.map((x) => {
            return {
                question: x.question,
                answer: x.answer,
                difficulty: x.difficulty
            }
        })
    }
    
    const options = {
        method: "PUT",
        body: JSON.stringify(flashcardSet),
        headers: {
            'Authorization': `Bearer ${token}`,
            "Content-type": "application/json; charset=UTF-8"
        }
    };
    
    const response = await fetch(url, options);
    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}