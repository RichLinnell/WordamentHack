# WordamentHack

This is for the PC version of the game.  

After playing Microsoft's Wordament online for a while, it became apparent to me that there is absolutely no way that the people who are coming in at the top of the leaderboards are playing "fair". So I thought I'd have a go at cheating myself. Guess what, I can now fairly reliably get position #1.

Initially I wrote a very basic, and hacky algorithm to just scan through the grid and find all words in a given dictionary and print them out. However, even if you did this, you couldn't type them in fast enough to get position #1, or actually anywhere near. At this point I was playing on my iPhone, so "sending" the words to the app was pretty much impossible.  I then discovered that on the PC version you can "type" the words in, so SendKeys became an option. This can now reliably get position #1.

The code uses a Scrabble dictionary that I found online.  It excludes all words of 2 or less letters.

Known issues :

>You have type the grid in yourself - it doesn't screen grab.
>It doesn't cater for the -ly, Lik- or XX type cells at the moment.  It really doent matter, just type in a number for these cells, you'll still win.
>It doesn't give the app the focus automatically.  Not sure why, I'll look at this next.
>The algo for finding the words needs a bit of a tidy up, it's bloated at the moment from my initial workings, I'll look at this next too.
