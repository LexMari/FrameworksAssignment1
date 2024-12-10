const apiBaseUrl = "https://localhost:7222/api/settings";

export async function getSettings(token) {
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

export async function updateSetting(token, key, setting) {
    const updateUrl = `${apiBaseUrl}/${key}`
    const options = {
        method: 'PUT',
        body: JSON.stringify(setting),
        headers: {
            'Authorization': `Bearer ${token}`,
            "Content-type": "application/json; charset=UTF-8"
        }
    };
    const response = await fetch(updateUrl, options);
    if (!response.ok) {
        if ([400, 404].includes(response.status)) {
            const error = await response.json();
            throw new Error(error.detail);
        }
        else
            throw new Error(`Error: ${response.status}`);
    }
    return response.json();
}