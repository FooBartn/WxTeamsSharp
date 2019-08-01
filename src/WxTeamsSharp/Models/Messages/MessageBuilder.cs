using System;
using System.Collections.Generic;
using WxTeamsSharp.Interfaces.Messages;

namespace WxTeamsSharp.Models.Messages
{
    /// <summary>
    /// Builder class for new messages. Start with MessageBuilder.New()
    /// </summary>
    public class MessageBuilder : ISendMessageWithFile, ISendMessageContent, ISendMessageTo
    {
        private string _toPersonId;
        private string _toPersonEmail;
        private string _text;
        private string _markdown;
        private string _localFile;
        private string _remoteFile;
        private bool _isLocalFile;
        private string _roomId;

        /// <inheritdoc/>
        public static ISendMessageTo New() => new MessageBuilder();

        /// <inheritdoc/>
        public ISendMessageContent SendToRoom(string roomId)
        {
            _roomId = roomId;
            return this;
        }

        /// <inheritdoc/>
        public ISendMessageContent SendToUserId(string toUserId)
        {
            _toPersonId = toUserId;
            return this;
        }

        /// <inheritdoc/>
        public ISendMessageContent SendToUserEmail(string toUserEmail)
        {
            _toPersonEmail = toUserEmail;
            return this;
        }

        /// <inheritdoc/>
        public ISendMessageWithFile WithText(string text)
        {
            _text = text;
            return this;
        }

        /// <inheritdoc/>
        public ISendMessageWithFile WithMarkdown(string markdown)
        {
            _markdown = markdown;
            return this;
        }

        /// <inheritdoc/>
        public IBuildableMessage WithLocalFile(string file)
        {
            _isLocalFile = true;
            _localFile = file;
            return this;
        }

        /// <inheritdoc/>
        public IBuildableMessage WithPublicFileUrl(string fileUrl)
        {
            _isLocalFile = false;
            _remoteFile = fileUrl;
            return this;
        }

        /// <inheritdoc/>
        public ISendableMessage Build()
        {
            if (string.IsNullOrEmpty(_markdown) && string.IsNullOrEmpty(_text))
                throw new ArgumentException("No valid message being sent");

            if (string.IsNullOrEmpty(_toPersonEmail) && string.IsNullOrEmpty(_toPersonId)
                && string.IsNullOrEmpty(_roomId))
                throw new ArgumentException("No valid recipient for message");

            var messageParams = new MessageParams(_isLocalFile)
            {
                Markdown = _markdown,
                Text = _text,
                ToPersonEmail = _toPersonEmail,
                ToPersonId = _toPersonId,
                RoomId = _roomId
            };

            if (!string.IsNullOrEmpty(_localFile) || !string.IsNullOrEmpty(_remoteFile))
                messageParams.Files = new List<string>();

            if (!string.IsNullOrEmpty(_localFile))
                messageParams.Files.Add(_localFile);

            if (!string.IsNullOrEmpty(_remoteFile))
                messageParams.Files.Add(_remoteFile);

            return messageParams;
        }
    }
}
