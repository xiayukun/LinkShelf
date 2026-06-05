# Windows 程序签名

English: [windows-code-signing.md](windows-code-signing.md)

程序签名指的是给 `LinkShelf.exe` 或安装包附加可信数字签名。这样 Windows 可以显示经过验证的发布者，而不是未知发布者；Microsoft Defender SmartScreen 也可以基于签名发布者和文件下载情况逐步积累信誉。

## 可选方式

- Microsoft Store MSIX：通过商店认证后由 Microsoft 给包签名。这对很多桌面应用是最干净的路径，但需要打包和提交商店。
- Microsoft Trusted Signing / Azure Artifact Signing：Microsoft 给非商店分发应用提供的托管签名服务。
- OV 或 EV 代码签名证书：从 Windows 信任的证书颁发机构购买证书，然后用 SignTool 签名。SmartScreen 仍然和发布者信誉、下载量等因素有关。
- 自签名证书：只适合本地测试。除非用户手动安装证书，否则其他用户的 Windows 不会默认信任它。

## 实际建议

对 Link Shelf 来说，在项目用户量还不大时，可以继续使用未签名的 GitHub 发布包。等项目有更多用户后，再评估签名成本或打包工作量。

比较现实的下一步：

1. 继续让 GitHub Actions 产出可复现发布包。
2. 给发布包增加校验和。
3. 公开使用量上来后，考虑 Microsoft Trusted Signing 或 OV 代码签名证书。
4. 如果需要更顺滑的大众安装体验，再考虑 MSIX 或 Microsoft Store。

## 参考

- Microsoft 代码签名选项：https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/code-signing-options
- MSIX 签名概览：https://learn.microsoft.com/en-us/windows/msix/package/signing-package-overview
- SignTool 包签名：https://learn.microsoft.com/en-us/windows/win32/appxpkg/how-to-sign-a-package-using-signtool
