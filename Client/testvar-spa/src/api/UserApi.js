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

export async function updateUser(token, userId, user) {
    const updateUrl = `${apiBaseUrl}/${userId}`
    const userData = {
        username: user.username,
        admin: user.admin
    }
    const options = {
        method: 'PUT',
        body: JSON.stringify(userData),
        headers: {
            'Authorization': `Bearer ${token}`,
            "Content-type": "application/json; charset=UTF-8"
        }
    };
    const response = await fetch(updateUrl, options);
    if (!response.ok) {
        if ([400, 403, 404].includes(response.status)) {
            const error = await response.json();
            throw new Error(error.title);
        }
        else
            throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}

export async function deleteUser(userId, token) {
    const deleteUrl = `${apiBaseUrl}/${userId}`
    const options = {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`,
        }
    };

    const response = await fetch(deleteUrl, options);
    if (response.status !== 204) {
        const problemDetail = response.json()
        throw new Error(`Error: ${problemDetail.detail}`);
    }
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
        const problemDetail = response.json()
        throw new Error(`Error: ${problemDetail.detail}`);
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
        const problemDetail = response.json()
        throw new Error(`Error: ${problemDetail.detail}`);
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
        const problemDetail = response.json()
        throw new Error(`Error: ${problemDetail.detail}`);
    }
    return response.json();
}


export async function updateUserCollection(userId, collection, token) {
    const updateUrl = `${apiBaseUrl}/${userId}/collections/${collection.id}`;
    const options = {
        method: 'PUT',
        body: JSON.stringify(collection),
        headers: {
            'Authorization': `Bearer ${token}`,
            "Content-type": "application/json; charset=UTF-8"
        }
    };

    const response = await fetch(updateUrl, options);
    if (!response.ok) {
        const problemDetail = response.json()
        throw new Error(`Error: ${problemDetail.detail}`);
    }
    return response.json();
}

export async function deleteUserCollection(userId, collectionId, token) {
    const collectionUrl = `${apiBaseUrl}/${userId}/collections/${collectionId}`;
    const options = {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    const response = await fetch(collectionUrl, options);
    if (response.status !== 204) {
        const problemDetail = response.json()
        throw new Error(`Error: ${problemDetail.detail}`);
    }
}

