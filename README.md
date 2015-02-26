# Fastest way to iterate files, folders and subfolders
Just some tests to figure out what would be the most efficient way of getting all files in directory and subdirectories. 
* If you need to list a full disk volume, I think the best method is usn-journal (for ntfs, not included in source)
* For just a part of the disk it seems a custom recursive parallel yields the best results.
* Since I am using this for file synchronisation to Azure, I need to have the path to the file and the last modified date.

Feel free to adjust and help me find faster ways...
