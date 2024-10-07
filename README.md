# Tardigrade

## Git Submodule Instructions

To set up:

```bash
# Clone repository
git clone --recursive git@github.com:cydtseng/Tardigrade.git

# Recursively update all submodules
git submodule update --recursive --remote
```

To update submodules:

```bash
# Force update and discard local changes
git submodule update --recursive --remote --force

# Can also use this if make is available—it calls the same command (but you must install on Windows):
make nuke
```

## FMOD Setup in Unity
Open Unity project > FMOD > in Studio Project Path, enter `Assets/Music/Tardigrade/Tardigrade.fspro`

No need to install FMOD studio or anything else.