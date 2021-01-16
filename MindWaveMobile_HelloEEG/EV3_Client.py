import socket
from ev3dev.ev3 import *

client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client_socket.connect(('165.132.138.195', 9999))
ev3 = ev3.dev

while True:
    data = client_socket.recv(1024).decode()
    print(data)
    if data == "":
    ev3.run()


mL = LargeMotor('outA'); mL.stop_action = 'hold'
mR = LargeMotor('outD'); mR.stop_action = 'hold'

mL.run_to_rel_pos(position_sp = 90, speed_sp = 500)
mR.run_to_rel_pos(position_sp = 90, speed_sp = 500)