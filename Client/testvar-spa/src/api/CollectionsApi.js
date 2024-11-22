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