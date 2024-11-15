const testvarAuthSettings = {
    authority: 'https://localhost:7222',
    client_id: 'TestVar.Spa',
    client_secret: '7E45AB83-BA83-471A-8041-698D4734F835',
    redirect_uri: 'http://localhost:3000/oauth/callback',
    silent_redirect_uri: 'http://localhost:3000/oauth/callback',
    post_logout_redirect_uri: 'http://localhost:3000/',
    response_type: 'code',
    scope: 'testvarapi',
    loadUserInfo: true
};

export const testvarAuthConfig = {
    settings: testvarAuthSettings,
    flow: 'testvar'
};