# GitHub 上线清单

这份清单用于把 Link Shelf 从本地工具整理成更容易信任、试用和收藏的仓库。

## 定位

开头使用最广泛有用的承诺：

> 把分散的本地路径移动到一个可携带文件夹中，并通过符号链接让应用继续工作。

Syncthing 应作为推荐同步搭档，而不是项目存在的唯一理由。

推荐 GitHub 主题：

- `windows`
- `symlink`
- `backup`
- `portable`
- `sync`
- `syncthing`
- `dotnet`
- `wpf`

## 发布前

- 用真实主窗口截图替换 `Assets/app-preview.png`。
- 确认发布产物名为 `LinkShelf.exe`。
- 确认 `README.md` 开头说明问题和快速开始。
- 确认 `LICENSE` 存在。
- 确认 `PRIVACY.md` 和 `PRIVACY.zh-CN.md` 存在，并说明本地运行行为。
- 按 `docs/first-push.zh-CN.md` 完成空 GitHub 仓库和首次推送。
- 确认 `.github/workflows/build.yml` 在 GitHub Actions 上通过。
- 创建首个发布版本并写简短更新说明。
- 使用 `docs/repository-profile.zh-CN.md` 中的仓库资料。

## 受欢迎仓库通常做得好的地方

清晰的第一屏：

- README 用一两句话说明问题。
- 主截图出现在靠前位置。
- 安装和快速开始说明不需要翻找。

低试用成本：

- 用户可以下载一个产物。
- 第一次命令或第一次点击很明确。
- 失败模式和限制有文档说明。

信任信号：

- 有许可证。
- 可以看到构建工作流。
- 有问题模板。
- 发布版本有名称和版本号。
- 危险行为被诚实说明。

社区准备：

- 有 `CONTRIBUTING.md` 和 `CONTRIBUTING.zh-CN.md`。
- 有安全报告路径。
- 维护者能快速回应早期问题。
- 路线图具体但不过度膨胀。

## 有用参考

- GitHub 开源指南：https://opensource.guide/building-community/
- GitHub README 研究：https://arxiv.org/abs/2206.10772
- README 结构和流行度研究：https://enoei.github.io/papers/liu2022readme.pdf

## 首个发布说明草稿

```text
Link Shelf 1.0.0

首个公开版本。

亮点：
- Windows 图形界面，用于把文件和目录移动到可携带缓存根目录。
- 可在另一台机器上恢复符号链接。
- 命令行健康检查支持 JSON 输出。
- 支持英文和中文界面，并可自动按系统语言选择。
- 英文配置架构，运行时目录名也使用英文。
- 适合 Syncthing 工作流，但不要求必须使用 Syncthing。
```
