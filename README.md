# NetScorpion
A Client-Server DDoS Attack Project

```cs
listen start                            Opens a listening port and starts listening for incomming connections.
listen stop                             Opens a listening port and starts listening for incomming connections.
stop                                    Stops the server.
list client                             Shows a list for all connected clients.
help                                    Shows this message.
attack start <ip> <port> <client/all>   Starts a TCP attack for all connected clients ('list client')
attack stop                             Stops current attack.
test connection                         Tests all connections.
options list                            Shows a list for all options (e.g. port, ip)
set <option> <value>                    Sets a global option. (e.g. 'set port 1234', 'set ip 127.0.0.1')
disconnect <ip:port/all>                Disconnects specified client.
```
