import os
import shutil

source_path = input("Path to files: ")
destination_path = input("Path to destination: ")

os.makedirs(destination_path, exist_ok=True)

for filename in os.listdir(source_path):
    if (filename.endswith(".asset") or filename.endswith(".asset.meta")):
        src = os.path.join(source_path, filename)
        dst = os.path.join(destination_path, filename)
        shutil.move(src, dst)
        print("Moved " + filename + " to " + destination_path)