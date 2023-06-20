export const environment = {
  production: false,
  apiGateway : 'http://localhost:5294/',
  keycloak: {
    url: 'https://login-keycloak.azurewebsites.net/auth',
    realm: 'booking-app',
    clientId: 'booking-app-client',
    accountUrl: 'https://login-keycloak.azurewebsites.net/auth/booking-app/account/#/personal-info'
  }
};
