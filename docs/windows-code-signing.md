# Windows Code Signing

Code signing means attaching a trusted digital signature to `LinkShelf.exe` or to an installer. Windows can then show a verified publisher instead of an unknown publisher, and Microsoft Defender SmartScreen can build reputation for the signed publisher and file.

## Options

- Microsoft Store MSIX: Microsoft signs the package after Store certification. This is the cleanest path for many desktop apps, but it requires packaging and Store submission.
- Microsoft Trusted Signing / Azure Artifact Signing: Microsoft's managed signing service for apps distributed outside the Store.
- OV or EV code signing certificate: Buy a certificate from a certificate authority trusted by Windows, then sign with SignTool. SmartScreen reputation still depends on publisher and download reputation.
- Self-signed certificate: useful for local testing only. It is not trusted by other users unless they manually install the certificate.

## Practical Recommendation

For Link Shelf, keep unsigned GitHub releases until the project has enough users to justify signing cost or packaging work.

Next realistic steps:

1. Keep release assets reproducible through GitHub Actions.
2. Add checksums to releases.
3. Consider Microsoft Trusted Signing or an OV code signing certificate when public adoption grows.
4. Consider MSIX or Microsoft Store only if the project needs a smoother mainstream install path.

## References

- Microsoft code signing options: https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/code-signing-options
- MSIX signing overview: https://learn.microsoft.com/en-us/windows/msix/package/signing-package-overview
- SignTool package signing: https://learn.microsoft.com/en-us/windows/win32/appxpkg/how-to-sign-a-package-using-signtool
