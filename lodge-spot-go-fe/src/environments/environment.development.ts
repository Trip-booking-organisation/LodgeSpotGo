export const environment = {
  production: false,
  apiGateway : 'http://localhost:5294/',
  keycloak: {
    url: 'http://localhost:8080',
    realm: 'lodge-spot-go',
    clientId: 'lodge-spot-go',
    accountUrl: 'http://localhost:8080/realms/lodge-spot-go/account/#/personal-info'
  }
};
