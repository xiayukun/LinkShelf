# 首次推送清单

English: [first-push.md](first-push.md)

创建名为 `LinkShelf` 的空 GitHub 仓库后，使用这份清单。

## GitHub 仓库设置

创建仓库时使用这些选项：

- 所有者：你的 GitHub 账号或组织
- 仓库名：`LinkShelf`
- 描述：`A Windows desktop and CLI tool for moving scattered files and folders into a portable cache root, then restoring them with symbolic links.`
- 可见性：`Public`
- 添加 README：关闭
- 添加 .gitignore：`No .gitignore`
- 添加许可证：`No license`

本地仓库已经包含 `README.md`、`.gitignore` 和 `LICENSE`，所以不要让 GitHub 自动生成这些文件。

## 本地 Git 设置

为这个仓库设置提交身份：

```powershell
git config user.name "xiayukun"
git config user.email "YOUR_GITHUB_EMAIL"
```

如果不想公开个人邮箱，可以使用 GitHub 邮箱设置中的 noreply 邮箱。

## 首次提交

```powershell
git status --short
git diff --cached --check
git commit -m "Initial public release"
```

## 连接远程仓库

把下面地址替换为 GitHub 显示的仓库地址：

```powershell
git remote add origin https://github.com/xiayukun/LinkShelf.git
git branch -M main
git push -u origin main
```

## 推送之后

- 确认 README 在 GitHub 上渲染正确。
- 确认截图显示在 README 靠前位置。
- 确认 GitHub Actions 中的构建工作流已开始运行。
- 添加 `docs/github-launch-checklist.zh-CN.md` 中列出的仓库主题。
- 使用 `release` GitHub Actions 工作流创建首个发布版本。分支使用 `main`，标签使用 `v1.0.0`。
