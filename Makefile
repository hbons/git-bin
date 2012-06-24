all:
	xbuild /property:Configuration=Release
	mv git-bin/bin/Release/git-bin.exe git-bin/bin/Release/git-bin

