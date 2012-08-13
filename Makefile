all:
	xbuild /property:Configuration=Release
	cp git-bin/git-bin.in git-bin/bin/Release/git-bin
	chmod +x git-bin/bin/Release/git-bin

