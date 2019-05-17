cd /d/Unity3d/NP
git add -A
echo -n "enter commit message: "
read commit_name
git commit -m "$commit_name"
git remote add origin https://github.com/lazarevd/AdventureUnity.git
git fetch
git push -u origin master