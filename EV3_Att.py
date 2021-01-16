import socket
import matplotlib.pyplot as plt

host = "0.0.0.0"
port = 8888

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((host, port))
server_socket.listen()

x=0
while True:
    client_socket, addr = server_socket.accept()
    while True:
        data = client_socket.recv(1024).decode()
        Att = int(''.join(list(filter(str.isdigit, data))))
        print(Att)
        x=x+1
        y=Att
        plt.title("Mindwave Attention data")
        #plt.scatter(x,y)
        plt.plot(x,y,'xb-')
        plt.pause(0.001)
        break
plt.show()
        

        
    #except KeyboardInterrupt:
    #   server_socket.close()
    #    print("Keyboard interrupt")
    #    break
    