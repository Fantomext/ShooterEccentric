import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("number")
    x = Math.floor(Math.random() * 50) - 25;

    @type("number")
    y = Math.floor(Math.random() * 50) - 25;

    @type("number")
    xSpeed = 0;

    @type("number")
    ySpeed = 0;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string) {
        this.players.set(sessionId, new Player());
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) {
        if (movement.x)
            this.players.get(sessionId).xSpeed = movement.x;
        if (movement.y)
            this.players.get(sessionId).ySpeed = movement.y;

    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 8;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            this.state.movePlayer(client.sessionId, data);
        });

        this.setSimulationInterval(() => this.update());
    }

    update()
    {
        this.state.players.forEach((player) => {
            player.x += player.xSpeed;
            player.y += player.ySpeed;

            console.log(player.x, ":", player.y);
        })
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client) {
        client.send("hello", "world");
        this.state.createPlayer(client.sessionId);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
