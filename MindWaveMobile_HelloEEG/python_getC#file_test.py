import subprocess
from subprocess import Popen, PIPE, STDOUT

p = Popen('HelloEEG.cs', stdout = PIPE, stderr = STDOUT, shell = True)

while True:
    line = p.stdout.readline()  # C#파일의 output을 'line' 변수에 저장.

    proc=subprocess.Popen('echo ''line', shell=True, stdout=subprocess.PIPE, )
    output=proc.communicate()[0]
    #print(output.decode('utf-8'))

    print(line.decode('utf-8'))                 # terminal에 line 출력.
    

    
