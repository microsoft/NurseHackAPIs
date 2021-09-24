# Project Nurse Hack API

[![Build and deploy ASP.Net Core app to Azure Web App - HackAPI-V2](https://github.com/microsoft/NurseHackAPIs/actions/workflows/main_HackAPI-V2.yml/badge.svg?branch=main)](https://github.com/microsoft/NurseHackAPIs/actions/workflows/main_HackAPI-V2.yml)

## Deployment Guide
- [ ] Create an Azure AD service principal with RBAC Contributor access to your target Azure Resourcew Group - follow [this guide](https://docs.microsoft.com/en-us/azure/app-service/deploy-github-actions?tabs=userlevel#tabpanel_1_userlevel).
- [ ] Update [main_HackAPI-V2.yml](./.github/workflows/main_HackAPI-V2.yml) - Set `Subscription` and `ResourceGroup` ENV variables.
- [ ] Create Azure AD App Registration for API app
- [ ] Update app service configuration

If implementing a logic app to call the API: 
- [ ] Hack API Application Registration must have an App Role defined (Read.Write.All)
- [ ] Logic App needs it's own Application Registration
- [ ] w\ reply url set to https://logic-apis-eastus.consent.azure-apim.net/redirect
- [ ] w\ application permissions (not delegate permissions) to hack api
- [ ] w\ the manifest containing the Hack API App Role created above

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
