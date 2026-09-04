# Script to create GitHub repository and push code

Write-Host "Step 1: Authenticating with GitHub..." -ForegroundColor Cyan
gh auth login

Write-Host "`nStep 2: Creating GitHub repository..." -ForegroundColor Cyan
gh repo create MNight-works --public --source=. --remote=origin --push

Write-Host "`nDone! Your repository has been created and pushed to GitHub." -ForegroundColor Green
Write-Host "Repository URL: https://github.com/$(gh api user -q .login)/MNight-works" -ForegroundColor Yellow
