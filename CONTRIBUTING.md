# Contributing to Dynamic Island for Windows

Thank you for your interest in contributing! Here's how to get started.

## Getting Started

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Make your changes
4. Test locally: `dotnet build && dotnet run`
5. Commit with a clear message: `git commit -m "Add: description of change"`
6. Push and open a Pull Request

## Guidelines

- **Keep it simple** — Small, focused PRs are easier to review
- **Follow existing patterns** — Match the layered architecture (Core → Services → UI)
- **Test your changes** — Make sure `dotnet build` succeeds and notifications still work
- **No breaking changes** — If your change affects existing behavior, document it clearly

## Architecture Rules

| Layer | Can depend on |
|-------|--------------|
| Core | Nothing (standalone) |
| Services | Core only |
| UI | Core only |
| App (root) | Everything |

## Reporting Issues

Open a GitHub Issue with:
- Windows version and build number
- Steps to reproduce
- Expected vs actual behavior
- Screenshots if applicable

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
