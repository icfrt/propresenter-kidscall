const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const path = require('path');
const cors = require('cors');

const app = express();
const server = http.createServer(app);
const io = socketIo(server, {
  cors: {
    origin: "*", // Allow all origins
    methods: ["GET", "POST"]
  }
});

app.use(cors()); 
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Serve static files from the "public" directory
app.use(express.static(path.join(__dirname, 'public')));

const config = require('./config.json');

const entries = [];

const web_clients = [];
const prop_clients = [];

const prop_code = config.prop_code || (Math.random() + 1).toString(36).substring(10);

const STATE_CREATED = 'created';
const STATE_SHOWING = 'showing';
const STATE_DECLINED = 'declined';
const STATE_DONE = 'done';
const STATE_OVERWRITTEN = 'overwritten';

function cleanupOldEntries() {
  const now = new Date();
  const timeout = 1000 * 60 * 60 * 24; // 24 hours

  for (let i = 0; i < entries.length; i++) {
    if (now - entries[i].timeStamp > timeout) {
      entries.splice(i, 1);
      i--;
    }
  }
}

function getLastEntry(state = null) {
  if (entries.length > 0) {
    const entry = entries[entries.length - 1];
    if (state === null || entry.state === state) {
      return entry;
    }
  }

  return null;
}

function getState() {
  return {
    prop: prop_clients.length > 0,
  }
}

setInterval(() => {
  cleanupOldEntries();
  io.emit('entries', entries);
}, 1000 * 60 * 10); // 10 minutes

io.on('connection', (socket) => {
  console.log('a user connected');

  web_clients.push(socket);

  socket.on('disconnect', () => {
    let index = web_clients.indexOf(socket);
    if (index > -1) web_clients.splice(index, 1);

    index = prop_clients.indexOf(socket);
    if (index > -1) {
      prop_clients.splice(index, 1);
      io.emit('state', getState());
    }

    console.log('user disconnected');
  });

  socket.on('authorize', (data) => {
    if (data.code === prop_code) {
      socket.emit('authorized', true);
      prop_clients.push(socket);

      io.emit('state', getState());
      
      let index = web_clients.indexOf(socket);
      if (index > -1) web_clients.splice(index, 1);

      socket.on('process', (data) => {
        const entry = entries.find(entry => entry.id === data.id);
        if (entry) {
          entry.state = data.state;
          io.emit('entry', entry);
        }
      });

      const lastEntry = getLastEntry(STATE_CREATED);
      if (lastEntry !== null) {
        socket.emit('entry', lastEntry);
      }

      console.log('prop connected');
    } else {
      socket.emit('authorized', false);
    }
  });

  socket.on('entry', (data) => {
    if (!data || !data.code || /^[0-9A-Za-z]+$/.test(data.code) === false) {
      return
    }

    const newEntry = {
      id: entries.length + 1,
      code: data.code.toUpperCase(),
      timeStamp: new Date(),
      state: STATE_CREATED
    }

    const lastEntry = getLastEntry(STATE_CREATED);
    if (lastEntry !== null) {
      lastEntry.state = STATE_OVERWRITTEN;
      io.emit('entry', lastEntry);
    }

    entries.push(newEntry);
    io.emit('entry', newEntry);
  });

  socket.on('entries', () => {
    socket.emit('entries', entries);
  });

  socket.on('state', () => {
    socket.emit('state', getState());
  });

  socket.emit('state', getState());
  socket.emit('entries', entries);
});

const PORT = process.env.PORT || 5000;

server.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});