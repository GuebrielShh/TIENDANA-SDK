public static class MercadoPagoConstants
{
    public const string BASE_URL = "https://api.mercadopago.com";
    public const string AUTH_BASE_URL = "https://auth.mercadopago.com";

    // OAuth
    public const string OAUTH_AUTHORIZE_ENDPOINT = "/authorization";
    public const string OAUTH_TOKEN_ENDPOINT = "/oauth/token";

    // Pagos y otros
    public const string PAYMENT_ENDPOINT = "/v1/payments";
    public const string PAYMENT_METHODS_ENDPOINT = "/v1/payment_methods";
    public const string IDENTIFICATION_TYPES_ENDPOINT = "/v1/identification_types";
    public const string FINANCIAL_INSTITUTIONS_ENDPOINT = "/v1/payment_methods/card_issuers";
    public const string PREFERENCE_ENDPOINT = "/checkout/preferences";
}
