update:
	git submodule update --recursive --remote

ignore-conflicts:
	git update-index --skip-worktree Assets/Prefabs/Player/Player.prefab
