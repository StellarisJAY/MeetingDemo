<script setup>
    import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr"

</script>

<template>
    <div>
        <p></p>
        <button @click="onCreateButtonClicked()">create pc</button>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                pc: null,
                hubConn: null,
            }
        },
        created() {
            
        },
        methods: {
            createHubConnection: async function () {
                const conn = new HubConnectionBuilder().withUrl("http://localhost:5059/sdp").withAutomaticReconnect().build();
                await conn.start();
                let _this = this;
                conn.on("SDPAnswer", args => {
                    _this.onServerSDPAnswer(args);
                });
                conn.on("ServerIceCandidate", args => {
                    _this.onServerIceCandidate(args);
                });
                conn.on("SdpExchangeError", args => {
                    console.error(args[0]);
                });
                return conn;
            },
            createPeerConnection: async function () {
                const hubConn = this.hubConn;
                let mediaStream = await navigator.mediaDevices.getUserMedia({ video: false, audio: true });
                let pc = new RTCPeerConnection({
                    iceServers: [
                        {
                            urls: "stun:stun.l.google.com:19302",
                        }
                    ]
                });
                pc.addTrack(mediaStream.getAudioTracks()[0], mediaStream)
                let offer = await pc.createOffer({
                    offerToReceiveAudio: true,
                    offerToReceiveVideo: true,
                });
                await pc.setLocalDescription(offer);
                await hubConn.send("SDPOffer", JSON.stringify(offer), 0, 0); // todo roomID and userID
                pc.onicecandidate = ev => {
                    if (ev.candidate != null) {
                        hubConn.send("IceCandidate", JSON.stringify(ev.candidate), 0, 0);
                    }
                };
                pc.onconnectionstatechange = ev => {
                    console.log(ev);
                };
                return pc
            },
            onCreateButtonClicked: function () {
                let _this = this;
                this.createHubConnection()
                    .then(conn => _this.hubConn = conn)
                    .then(_ => { return this.createPeerConnection(); })
                    .then(pc => console.log(pc))
                    .catch(err => console.error(err));
            },
            onServerIceCandidate: function (args) {
                
            },
            onServerSDPAnswer: function (args) {

            },
        },
    }
</script>
