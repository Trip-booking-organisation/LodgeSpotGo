export const environment = {
  production: false,
  apiGateway : 'http://localhost:5294',
  keycloak: {
    url: 'http://localhost:8080',
    realm: 'booking-app',
    clientId: 'booking-app-client',
    accountUrl: 'http://localhost:8080/realms/booking-app/account/#/personal-info'
  }
};
