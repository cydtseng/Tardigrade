diff:
	cd Assets/Art \
		&& git diff

# git-stash is used to clean up repo for pulls, and subsequently
# to prevent auto-committing metadata file modifications. Commit
# and push will only succeed if files have been staged.
update:
	cd Assets/Music \
		&& git checkout main \
		&& git pull
	cd Assets/Art \
		&& git stash -u \
		&& git checkout main \
		&& git pull \
		&& git stash pop \
		&& git stash \
		&& git add . \
		&& git stash pop \
		&& git commit -m "minor: update metadata" \
		&& git push

nuke:
	git submodule update --recursive --remote --force
nukeall:
	git lfs migrate import --fixup --everything
