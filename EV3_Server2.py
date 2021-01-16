import socket
from ev3dev.ev3 import *


host = "0.0.0.0"
port = 9999
mL = LargeMotor('outA')
mR = LargeMotor('outB')
mL.stop_action = 'hold'
mR.stop_action = 'hold'


server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((host, port))
server_socket.listen()

Blink_count = 0


while True:
    client_socket, addr = server_socket.accept()
    while True:
        data = client_socket.recv(1024).decode()
        print(data)
        Att = int(''.join(list(filter(str.isdigit, data))))
        print(Att)
        Att_index = data.find('Att')
        #Med_index = data.find('Med')
        
        if 'Blink' in data:
            Blink_count += 1
            print('Blink Detected:'+ str(Blink_count))
            if Blink_count == 2 :
                print('Turn 45 Deg. !!')
                mL.run_to_rel_pos(position_sp = -240, speed_sp = 600)
                mR.run_to_rel_pos(position_sp = 240, speed_sp = 600)
                Blink_count = 0
        else :
            if Att >= 70:
                print('Go Forward!!' + str(Att))
                mL.run_to_rel_pos(position_sp = 360, speed_sp = 600)
                mR.run_to_rel_pos(position_sp = 360, speed_sp = 600)
                Blink_count = 0
                
            elif Att < 30:
                print('Go Backward!!' + str(Att))
                mL.run_to_rel_pos(position_sp = -360, speed_sp = 600)
                mR.run_to_rel_pos(position_sp = -360, speed_sp = 600)
                Blink_count = 0
            else :
                Blink_count=0
        
        break
        # except KeyboardInterrupt:
        #     server_socket.close()
        #     print("Keyboard interrupt")
        #     break
