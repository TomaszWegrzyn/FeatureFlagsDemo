Prerequisites:
1. Consul installed and running on localhost:8500
2. Configure OpenId Connect in appsettings:
```json
"OpenIdConnect": {
    "ClientId": "THIS SHOULD BE A SECRET",
    "ProviderAddress": "URL" //example https://dev-sometenant.us.auth0.com
}
```
3. OIDC provider should include role(s) as defined by ClaimTypes.Role.