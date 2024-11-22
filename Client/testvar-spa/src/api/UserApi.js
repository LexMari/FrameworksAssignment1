const apiBaseUrl = "https://localhost:7222/api/users";

export async function getUsers(token) {
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

export async function createUser(userdata) {
    const options = {
        method: 'POST',
        body: JSON.stringify(userdata),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    };

    const response = await fetch(apiBaseUrl, options);
    if (!response.ok) {
        if (response.status === 400) {
            const error = await response.json();
            throw new Error(error.message);
        }
        else
            throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}

export async function getUserFlashcardSets(userId, token) {
    const userSetsUrl = `${apiBaseUrl}/${userId}/sets`
    const options = {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    const response = await fetch(userSetsUrl, options);
    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}

export async function getUserCollections(userId, token) {
    const collectionsUrl = `${apiBaseUrl}/${userId}/collections`
    const options = {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    const response = await fetch(collectionsUrl, options);
    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}

export async function getUserCollection(userId, collectionId, token) {
    const collectionUrl = `${apiBaseUrl}/${userId}/collections/${collectionId}`;
    const options = {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    const response = await fetch(collectionUrl, options);
    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}

