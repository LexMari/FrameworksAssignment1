# TestVar Flashcard Client Application

The client is build on the React framework and makes use of the Material UI theme for design elements.

## Authentication

The application is confiured as an OAUTH client to the ApiServer that provides and identity services that
implements the OAUTH protocol using the .Net OpenIdDict libraries. Because the service is OAUTH compliant
this client application uses the `oidc-auth-ts` library to handle the authentication and token exchange
processes.

The implementation of both parts is based on the following source article https://andreyka26.com/oauth-authorization-code-react-client-pt1-openIddict