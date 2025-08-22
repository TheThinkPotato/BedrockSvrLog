const dgram = require("dgram");
const client = dgram.createSocket("udp4");

const message = Buffer.from("Hello from JS");
client.send(message, 5550, "127.0.0.1", (err) => {
  if (err) throw err;
  console.log("Message sent");
});

client.on("message", (msg, rinfo) => {
  console.log(`Received from C#: ${msg.toString()}`);
  client.close();
});
