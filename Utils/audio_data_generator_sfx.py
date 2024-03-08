import os
import re

def create_audio_data(directory):
    asset_content_template = """%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: 0}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: 623d13fb76d0411c988414b705ba7e51, type: 3}}
  m_Name: {file_name}
  m_EditorClassIdentifier: 
  clip: {{fileID: 8300000, guid: {guid}, type: 3}}
  mixerGroup: {{fileID: -5388765665678994932, guid: 6b885a192e05b3d4dac2c81155d11090, type: 2}}
  playOnAwake: 0
  loop: 0
  volume: 1
    """

    for filename in os.listdir(directory):
        if filename.endswith(".wav"):
            meta_filename = filename + ".meta"
            meta_file_path = os.path.join(directory, meta_filename)

            if os.path.exists(meta_file_path):
                with open(meta_file_path, 'r') as meta_file:
                    meta_content = meta_file.read()
                    match = re.search(r'guid: ([a-z0-9]+)', meta_content)
                    if match:
                        guid = match.group(1)

                        file_base_name = filename.split('.')[0]
                        asset_content = asset_content_template.format(file_name=file_base_name, guid=guid)
                        
                        asset_file_path = os.path.join(directory, filename.replace('.wav', '.asset'))
                        with open(asset_file_path, 'w') as asset_file:
                            asset_file.write(asset_content)

def main():
    directory_path = input("Path to .wav files: ")
    create_audio_data(directory_path)

if __name__ == "__main__":
    main()