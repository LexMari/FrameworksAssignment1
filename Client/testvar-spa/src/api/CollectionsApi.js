const apiBaseUrl = "https://localhost:7222/api/collections";

export async function getCollections(token) {
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

export async function createCollection(name, token) {
    const options = {
        method: 'POST',
        body: JSON.stringify({comment: name}),
        headers: {
            'Authorization' : `Bearer ${token}`,
            "Content-type" : "application/json; charset=UTF-8"
        }
    };
    
    const response = await fetch(apiBaseUrl, options);
    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}