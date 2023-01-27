using System.Text.Json.Serialization;

namespace Northwind.Api.Client
{
    internal class OpenIdConfigurationModel
    {
        public OpenIdConfigurationModel()
        {
            this.BackchannelTokenDeliveryModesSupported = Array.Empty<string>();
            this.ScopesSupported = Array.Empty<string>();
            this.ClaimsSupported = Array.Empty<string>();
            this.GrantTypesSupported = Array.Empty<string>();
            this.ResponseTypesSupported = Array.Empty<string>();
            this.ResponseModesSupported = Array.Empty<string>();
            this.TokenEndpointAuthMethodsSupported = Array.Empty<string>();
            this.IdTokenSigningAlgValuesSupported = Array.Empty<string>();
            this.SubjectTypesSupported = Array.Empty<string>();
            this.CodeChallengeMethodsSupported = Array.Empty<string>();
            this.RequestObjectSigningAlgValuesSupported = Array.Empty<string>();
            this.BackchannelTokenDeliveryModesSupported = Array.Empty<string>();
            this.Issuer = string.Empty;
            this.JwksUri = string.Empty;
            this.AuthorizationEndpoint = string.Empty;
            this.TokenEndpoint = string.Empty;
            this.UserinfoEndpoint = string.Empty;
            this.CheckSessionIframe = string.Empty;
            this.RevocationEndpoint = string.Empty;
            this.IntrospectionEndpoint = string.Empty;
            this.DeviceAuthorizationEndpoint = string.Empty;
            this.BackchannelAuthenticationEndpoint = string.Empty;
            this.EndSessionEndpoint = string.Empty;
        }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }
        
        [JsonPropertyName("jwks_uri")]
        public string JwksUri { get; set; }
        
        [JsonPropertyName("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }
        
        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; }
        
        [JsonPropertyName("userinfo_endpoint")]
        public string UserinfoEndpoint { get; set; }
        
        [JsonPropertyName("end_session_endpoint")]
        public string EndSessionEndpoint { get; set; }
        
        [JsonPropertyName("check_session_iframe")]
        public string CheckSessionIframe { get; set; }
        
        [JsonPropertyName("revocation_endpoint")]
        public string RevocationEndpoint { get; set; }
        
        [JsonPropertyName("introspection_endpoint")]
        public string IntrospectionEndpoint { get; set; }
        
        [JsonPropertyName("device_authorization_endpoint")]
        public string DeviceAuthorizationEndpoint { get; set; }
        
        [JsonPropertyName("backchannel_authentication_endpoint")]
        public string BackchannelAuthenticationEndpoint { get; set; }
        
        [JsonPropertyName("frontchannel_logout_supported")]
        public bool FrontchannelLogoutSupported { get; set; }
        
        [JsonPropertyName("frontchannel_logout_session_supported")]
        public bool FrontchannelLogoutSessionSupported { get; set; }
        
        [JsonPropertyName("backchannel_logout_supported")]
        public bool BackchannelLogoutSupported { get; set; }
        
        [JsonPropertyName("backchannel_logout_session_supported")]
        public bool BackchannelLogoutSessionSupported { get; set; }

        [JsonPropertyName("scopes_supported")]
        public string[] ScopesSupported { get; set; }
        
        [JsonPropertyName("claims_supported")]
        public string[] ClaimsSupported { get; set; }
        
        [JsonPropertyName("grant_types_supported")]
        public string[] GrantTypesSupported { get; set; }
        
        [JsonPropertyName("response_types_supported")]
        public string[] ResponseTypesSupported { get; set; }
        
        [JsonPropertyName("response_modes_supported")]
        public string[] ResponseModesSupported { get; set; }

        [JsonPropertyName("token_endpoint_auth_methods_supported")]
        public string[] TokenEndpointAuthMethodsSupported { get; set; }
        
        [JsonPropertyName("id_token_signing_alg_values_supported")]
        public string[] IdTokenSigningAlgValuesSupported { get; set; }
        
        [JsonPropertyName("subject_types_supported")]
        public string[] SubjectTypesSupported { get; set; }
        
        [JsonPropertyName("code_challenge_methods_supported")]
        public string[] CodeChallengeMethodsSupported { get; set; }
        
        [JsonPropertyName("request_parameter_supported")]
        public bool RequestParameterSupported { get; set; }
        
        [JsonPropertyName("request_object_signing_alg_values_supported")]
        public string[] RequestObjectSigningAlgValuesSupported { get; set; }
        
        [JsonPropertyName("authorization_response_iss_parameter_supported")]
        public bool AuthorizationResponseIssParameterSupported { get; set; }
        
        [JsonPropertyName("backchannel_token_delivery_modes_supported")]
        public string[] BackchannelTokenDeliveryModesSupported { get; set; }
        
        [JsonPropertyName("backchannel_user_code_parameter_supported")]
        public bool BackchannelUserCodeParameterSupported { get; set; }
    }
}
