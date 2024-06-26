using SIPSorcery.Net;
using System.Net;

namespace Meeting.Core.Meeting
{
    public class UserConnection
    {
        /// <summary>
        /// 1. Client sends SDP Offer using websocket
        /// 2. Server creates PeerConnection and replies SDP Answer
        /// 3. Server adds new Peer's track to other Peer Connections
        /// 4. Server adds tracks to new PeerConnection
        /// </summary>
        public RTCPeerConnection PeerConnection { get; private set; }
        public long ID { get; private set; }
        public string SDPAnswerString { get; private set; } = string.Empty;

        private UserConnection(RTCPeerConnection peerConnection)
        {
            PeerConnection = peerConnection;
        }

        public static UserConnection? CreateUserConnection(long userID, string sdpOfferString)
        {
            RTCPeerConnection peerConnection = new RTCPeerConnection();
            // 解析SDP Offer，创建SDP Answer
            bool flag = RTCSessionDescriptionInit.TryParse(sdpOfferString, out RTCSessionDescriptionInit sdpOffer);
            if (!flag)
            {
                return null;
            }
            peerConnection.setRemoteDescription(sdpOffer);
            var sdpAnswer = peerConnection.createAnswer();
            peerConnection.setLocalDescription(sdpAnswer);
            string sdpAnswerString = sdpAnswer.toJSON();

            var conn = new UserConnection(peerConnection)
            {
                ID = userID,
                SDPAnswerString = sdpAnswerString,
            };
            return conn;
        }

        public bool AddICECandidate(string candidateString)
        {
            bool flag = RTCIceCandidateInit.TryParse(candidateString, out  RTCIceCandidateInit iceCandidate);
            if (flag)
            {
                PeerConnection.addIceCandidate(iceCandidate);
            }
            return flag;
        }

        public void SetRTPPacketReceiver(Action<IPEndPoint, SDPMediaTypesEnum, RTPPacket> receiver)
        {
            PeerConnection.OnRtpPacketReceived += receiver;
        }

        public void SetOnIceCandidate(Action<RTCIceCandidate> onCandidate)
        {
            PeerConnection.onicecandidate += onCandidate;
        }
    }
}
