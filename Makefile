update:
	git submodule update --recursive --remote --force

ignore-conflicts:
	git update-index --skip-worktree Assets/Prefabs/Player/Player.prefab

reset-nuke:
	git lfs migrate import --fixup --everything
