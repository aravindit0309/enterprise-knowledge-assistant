Decision: Use AWS SDK dependency injection (AddAWSService<IAmazonBedrockRuntime>()) instead of manually instantiating AmazonBedrockRuntimeClient.

Rationale:

Centralizes AWS client configuration.
Leverages the built-in dependency injection ecosystem.
Makes testing and future configuration changes easier.
Keeps client creation out of business logic.