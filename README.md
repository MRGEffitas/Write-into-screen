Write-into-screen
=================

This program can be used to simulate keyboard events, and transfer files through the clipboard.

The program reads the config.json file, and applies the commands from it.

The valid JSON commands are:

text_type
normal text to be typed, with RETURN at the end
    {        "text_type": "winword"    },

text_type_without_return
normal text to be typed, without RETURN at the end
    {        "text_type_without_return": "winword"    },

sleep
sleep for this amount of 
{        "sleep": "5000"    },


file_base64_into_clipboard
read this file, convert to base64 and paste into clipboard
{        "file_base64_into_clipboard": "F:\\hacking\\dropper.zip"    },

zip_directory
zip the contents of the directory, and put it into dropper.zip, one directory aboves
	{		"zip_directory":"c:\\hacking\\todrop"		}

file_into_clipboard
read file into clipboard, use it for non-binary files
	{        "file_into_clipboard": "F:\\hacking\\hack.txt"    },

file_into_hex_into_clipboard
read (binary) file, convert into hex, paste into clipboard
{        "file_into_hex_into_clipboard": "F:\\hacking\\dropper.zip"    },

key_combo1
simulate the press of one key
			    {
        "key_combo1": [
            {
                "name": "DOWN"
            }
        ]
    },

key_combo2
simulate the simultan press of two keys, e.g. win + r
	{
        "key_combo2": [
            {                "name": "LWIN"            }   ,
			{                "name": "VK_R"            } 
		]    },

key_combo3
simulate the simultan press of three keys, e.g. alt + (i + m)
    {
        "key_combo3": [
            {
                "name": "LMENU"
            },
            {
                "name": "VK_I"
            },
            {
                "name": "VK_M"
            }
        ]
    },


key_combo3_2
simulate the simultan press of three keys, e.g. (ctrl + shift) + v

