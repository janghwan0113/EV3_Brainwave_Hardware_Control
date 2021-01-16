import socket
import threading


host = "0.0.0.0"
port = 9999


def handle_receive(_socket, num):

    while True:
        length = recvall(_socket, 16)
        string_data = recvall(_socket, int(length))
        data = np.frombuffer(string_data, dtype='uint8')
frame = cv2.imdecode(data, cv2.IMREAD_COLOR)
cv2.imwrite(f"./images/{count}.jpg", frame)


def handle_command():
global command, command_target
while True:
try:
command = input("please enter drone number and command : ")
print(command_target, command)
except:
pass


def accept_func():
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((host, port))
server_socket.listen(5)
drone_num = 1
command_thread = threading.Thread(target=handle_command)
command_thread.daemon = True
command_thread.start()

while True:
try:
drone_num += 1
client_socket, addr = server_socket.accept()
except KeyboardInterrupt:
server_socket.close()
print("Keyboard interrupt")
break

receive_thread = threading.Thread(target=handle_receive, args=(client_socket, drone_num))
receive_thread.daemon = True
receive_thread.start()


if __name__ == '__main__':
accept_func()