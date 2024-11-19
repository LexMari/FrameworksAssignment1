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

export async function createUser(token, userData) {
    const options = {
        method: 'POST',
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