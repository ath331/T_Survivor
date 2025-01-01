import argparse
import jinja2
import ProtoParser
import os


def main():

	arg_parser = argparse.ArgumentParser(description = 'PacketGenerator')
	arg_parser.add_argument('--path', type=str, default='../../GameServer/Packet/Protocol.proto', help='proto path')
	arg_parser.add_argument('--isRecvHandler', type=bool, default=False, help='isRecvHandler')
	arg_parser.add_argument('--recvHandlerPath', type=str, default='../../../GameServer/Packet/Handler', help='recvHandler path')
	arg_parser.add_argument('--sendHandlerPath', type=str, default='../../../AtClient/Source/AtClient/Packet/Handler', help='sendHandler path')
	arg_parser.add_argument('--output', type=str, default='TestPacketHandler', help='output file')
	arg_parser.add_argument('--recv', type=str, default='C_', help='recv convention')
	arg_parser.add_argument('--send', type=str, default='S_', help='send convention')

	arg_parser.add_argument('--UnityNetworkPath', type=str, default='../../../AtClientFramework/Assets/Scripts/Network', help='unity network path')
	arg_parser.add_argument('--OnlyUnityPacket', type=bool, default=False, help='OnlyUnityPacket')

	args = arg_parser.parse_args()

	parser = ProtoParser.ProtoParser(1000, args.recv, args.send)
	parser.parse_proto(args.path)

	file_loader = jinja2.FileSystemLoader('Templates')
	env = jinja2.Environment(loader=file_loader)

	if args.OnlyUnityPacket == False:
		template = env.get_template('PacketHandler.h')
		output = template.render(parser=parser, output=args.output)

		f = open(args.output+'.h', 'w+')
		f.write(output)
		f.close()

		print(output)

		# Each RecvPacket Handler Make
		if args.isRecvHandler:
			for index, recvPacket in enumerate(parser.recv_pkt):

				# Make HandlerTemplate ( forceMake )
				recvPacketHandlerTemplate = env.get_template('ClientPacketHandlerTemplate.h')
				eachHandler = recvPacketHandlerTemplate.render(pkt=recvPacket, output=args.recvHandlerPath)

				f = open(args.recvHandlerPath + '/' + recvPacket.name  +'HandlerTemplate.cpp', 'w+')
				f.write(eachHandler)
				f.close()

				print(eachHandler)


				# Make Dictory ( isNoHave )
				path = args.recvHandlerPath + '/'+ recvPacket.path
				if not os.path.exists(path):
					os.makedirs(path)


				# Make Handler.h ( isNoHave )
				recvPacketHandlerHeader = env.get_template('ClientPacketHandler.h')
				eachHandler = recvPacketHandlerHeader.render(pkt=recvPacket, output=args.recvHandlerPath)

				if not os.path.exists(path + '/' + recvPacket.name  +'Handler.h'):
					f = open(path + '/' + recvPacket.name  +'Handler.h', 'w+')
					f.write(eachHandler)
					f.close()

				print(eachHandler)


				# Make Handler.cpp ( isNoHave )
				recvPacketHandlerCPP = env.get_template('ClientPacketHandler.cpp')
				eachHandler = recvPacketHandlerCPP.render(pkt=recvPacket, output=args.recvHandlerPath)

				if not os.path.exists(path + '/' + recvPacket.name  +'Handler.cpp'):
					f = open(path + '/' + recvPacket.name  +'Handler.cpp', 'w+')
					f.write(eachHandler)
					f.close()

				print(eachHandler)



		# Each SendPacket Handler Make
		if args.isRecvHandler == False:
			# sendPacket. but for-each recv_pkt list. ( client and server change )
			for index, sendPacket in enumerate(parser.recv_pkt):

				# Make HandlerTemplate ( forceMake )
				sendPacketHandlerTemplate = env.get_template('ServerPacketHandlerTemplate.h')
				eachHandler = sendPacketHandlerTemplate.render(pkt=sendPacket, output=args.sendHandlerPath)

				f = open(args.sendHandlerPath + '/' + sendPacket.name  +'HandlerTemplate.cpp', 'w+')
				f.write(eachHandler)
				f.close()

				print(eachHandler)


				# MakeDictory ( isNoHave )
				path = args.sendHandlerPath + '/'+ sendPacket.path
				if not os.path.exists(path):
					os.makedirs(path)


				# Make Handler.h ( isNoHave )
				sendPacketHandlerHeader = env.get_template('ServerPacketHandler.h')
				eachHandler = sendPacketHandlerHeader.render(pkt=sendPacket, output=args.sendHandlerPath)

				if not os.path.exists(path + '/' + sendPacket.name  +'Handler.h'):
					f = open(path + '/' + sendPacket.name  +'Handler.h', 'w+')
					f.write(eachHandler)
					f.close()

				print(eachHandler)


				# Make Handler.cpp ( isNoHave )
				sendPacketHandlerHeader = env.get_template('ServerPacketHandler.cpp')
				eachHandler = sendPacketHandlerHeader.render(pkt=sendPacket, output=args.sendHandlerPath)

				if not os.path.exists(path + '/' + sendPacket.name  +'Handler.cpp'):
					f = open(path + '/' + sendPacket.name  +'Handler.cpp', 'w+')
					f.write(eachHandler)
					f.close()

				print(eachHandler)

	else:
####################################################################################################
#### @brief ServerPacketHandler In C#
####################################################################################################
		# PacketHandler ( in Unity )
		template = env.get_template('PacketIdInCS.h')
		output = template.render(parser=parser, output=args.UnityNetworkPath)

		f = open(args.UnityNetworkPath + '/PacketId.cs', 'w+')
		f.write(output)
		f.close()

		print(output)

		template = env.get_template('PacketHandlerInCS.h')
		output = template.render(parser=parser, output=args.UnityNetworkPath)

		f = open(args.UnityNetworkPath + '/Handler/PacketHandler.cs', 'w+')
		f.write(output)
		f.close()

		print(output)

		# SendPacket name is 'S_'
		for index, sendPacket in enumerate(parser.send_pkt):

			# MakeDictory ( isNoHave )
			path = args.UnityNetworkPath + '/Handler/'+ sendPacket.path
			if not os.path.exists(path):
				os.makedirs(path)

			# Make Handler.h ( isNoHave )
			sendPacketHandlerHeader = env.get_template('EachPacketHandlerInCS.h')
			eachHandler = sendPacketHandlerHeader.render(pkt=sendPacket)

			if not os.path.exists(path + '/' +'Packet_' + sendPacket.name  +'Handler.cs'):
				f = open(path + '/' + 'Packet_' + sendPacket.name  +'Handler.cs', 'w+')
				f.write(eachHandler)
				f.close()

			print(eachHandler)

		# RecvPacket name is 'C_'
		for index, recvPacket in enumerate(parser.recv_pkt):

			# MakeDictory ( isNoHave )
			path = args.UnityNetworkPath + '/Packet/'+ recvPacket.path
			if not os.path.exists(path):
				os.makedirs(path)

			# Make Packet.h ( isNoHave )
			eachPacket = env.get_template('EachPackeInCS.h')
			eachPacketRender = eachPacket.render(pkt=recvPacket)

			if not os.path.exists(path + '/' +'Packet_' + recvPacket.name  +'.cs'):
				f = open(path + '/' + 'Packet_' + recvPacket.name  +'.cs', 'w+')
				f.write(eachPacketRender)
				f.close()

			print(eachPacketRender)

####################################################################################################

	return

if __name__ == '__main__':
	main()