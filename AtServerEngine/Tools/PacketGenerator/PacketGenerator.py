import argparse
import jinja2
import ProtoParser
import os


def main():

	arg_parser = argparse.ArgumentParser(description = 'PacketGenerator')
	arg_parser.add_argument('--isPrint',                 type=bool, default=False,                                               help='Print OutPut?'              )

	arg_parser.add_argument('--protoPath',               type=str,  default='../../Network/Protocol.proto',                      help='proto path'                 )
	arg_parser.add_argument('--serverPacketHandler',     type=str,  default='ServerPacketHandler',                               help='Handler Templete in Clientg')
	arg_parser.add_argument('--serverPacketHandlerPath', type=str,  default='Test/ServerHandler',                                help='serverHandler path'         )
	arg_parser.add_argument('--clientPacketHandler',     type=str,  default='ClientPacketHandler',                               help='Handler Templete in Server' )
	arg_parser.add_argument('--clientPacketHandlerPath', type=str,  default='Test/ClientHandler',                                help='clientHandler path'         )
	arg_parser.add_argument('--recv',                    type=str,  default='C_,CT_',                                            help='recv convention'            )
	arg_parser.add_argument('--send',                    type=str,  default='S_,ST_',                                            help='send convention'            )

	arg_parser.add_argument('--UnityNetworkPath',        type=str,  default='Test/AtClientFramework/Assets/Scripts/Network',     help='unity network path'         )

	args = arg_parser.parse_args()

	parser = ProtoParser.ProtoParser(1000, args.recv, args.send)
	parser.parse_proto(args.protoPath)

	file_loader = jinja2.FileSystemLoader('Templates')
	env = jinja2.Environment(loader=file_loader)

	template = env.get_template('PacketHandler.h')
	output = template.render(parser=parser, output=args.clientPacketHandler)

	f = open(args.clientPacketHandler+'.h', 'w+')
	f.write(output)
	f.close()

	if args.isPrint:
		print(output)

	packetId = env.get_template('PacketId.h')
	packetIdOutput = packetId.render(parser=parser)

	f = open('PacketId.h', 'w+')
	f.write(packetIdOutput)
	f.close()

	if args.isPrint:
		print(packetIdOutput)

######## Each RecvPacket Handler Make
	print( "\033[32m----------Start Recv Packet---------- \033[0m" + "  count : " + str(len(parser.recv_pkt) ))

	for index, recvPacket in enumerate(parser.recv_pkt):

		# Make HandlerTemplate ( forceMake )
		recvPacketHandlerTemplate = env.get_template( args.clientPacketHandler +'Template.h')
		eachHandler = recvPacketHandlerTemplate.render(pkt=recvPacket, output=args.clientPacketHandlerPath)

		f = open(args.clientPacketHandlerPath + '/' + recvPacket.name  +'HandlerTemplate.cpp', 'w+')
		f.write(eachHandler)
		f.close()

		if args.isPrint:
			print(eachHandler)


		# Make Dictory ( isNoHave )
		path = args.clientPacketHandlerPath + '/'+ recvPacket.path
		if not os.path.exists(path):
			os.makedirs(path)


		# Make Handler.h ( isNoHave )
		recvPacketHandlerHeader = env.get_template( args.clientPacketHandler +'.h')
		eachHandler = recvPacketHandlerHeader.render(pkt=recvPacket, output=args.clientPacketHandlerPath)

		if not os.path.exists(path + '/' + recvPacket.name  +'Handler.h'):
			f = open(path + '/' + recvPacket.name  +'Handler.h', 'w+')
			f.write(eachHandler)
			f.close()

		if args.isPrint:
			print(eachHandler)


		# Make Handler.cpp ( isNoHave )
		recvPacketHandlerCPP = env.get_template( args.clientPacketHandler + '.cpp')
		eachHandler = recvPacketHandlerCPP.render(pkt=recvPacket, output=args.clientPacketHandlerPath)

		if not os.path.exists(path + '/' + recvPacket.name  +'Handler.cpp'):
			f = open(path + '/' + recvPacket.name  +'Handler.cpp', 'w+')
			f.write(eachHandler)
			f.close()

		if args.isPrint:
			print(eachHandler)

	print( "\033[32m----------End Recv Packet----------\033[0m" )

######### Each SendPacket Handler Make
	print( "\033[32m----------Start Send Packet---------- \033[0m" + "  count : " + str(len(parser.send_pkt) ))

	# sendPacket.
	for index, sendPacket in enumerate(parser.send_pkt):
	
		# Make HandlerTemplate ( forceMake )
		sendPacketHandlerTemplate = env.get_template(args.serverPacketHandler+'.h')
		eachHandler = sendPacketHandlerTemplate.render(pkt=sendPacket, output=args.serverPacketHandlerPath)
	
		f = open(args.serverPacketHandlerPath + '/' + sendPacket.name  +'HandlerTemplate.cpp', 'w+')
		f.write(eachHandler)
		f.close()
	
		if args.isPrint:
			print(eachHandler)
	
	
		# MakeDictory ( isNoHave )
		path = args.serverPacketHandlerPath + '/'+ sendPacket.path
		if not os.path.exists(path):
			os.makedirs(path)
	
		# Make Handler.h ( isNoHave )
		sendPacketHandlerHeader = env.get_template('ServerPacketHandler.h')
		eachHandler = sendPacketHandlerHeader.render(pkt=sendPacket, output=args.serverPacketHandlerPath)
	
		if not os.path.exists(path + '/' + sendPacket.name  +'Handler.h'):
			f = open(path + '/' + sendPacket.name  +'Handler.h', 'w+')
			f.write(eachHandler)
			f.close()
	
		if args.isPrint:
			print(eachHandler)
	
	
		# Make Handler.cpp ( isNoHave )
		sendPacketHandlerHeader = env.get_template('ServerPacketHandler.cpp')
		eachHandler = sendPacketHandlerHeader.render(pkt=sendPacket, output=args.serverPacketHandlerPath)
	
		if not os.path.exists(path + '/' + sendPacket.name  +'Handler.cpp'):
			f = open(path + '/' + sendPacket.name  +'Handler.cpp', 'w+')
			f.write(eachHandler)
			f.close()
	
		if args.isPrint:
			print(eachHandler)

	print( "\033[32m----------End Send Packet----------\033[0m" )

####################################################################################################
#### @brief ServerPacketHandler In C#
####################################################################################################
	# PacketHandler ( in Unity )
	template = env.get_template('PacketIdInCS.h')
	output = template.render(parser=parser, output=args.UnityNetworkPath)

	f = open(args.UnityNetworkPath + '/PacketId.cs', 'w+')
	f.write(output)
	f.close()

	if args.isPrint:
		print(output)

	template = env.get_template('PacketHandlerInCS.h')
	output = template.render(parser=parser, output=args.UnityNetworkPath)

	f = open(args.UnityNetworkPath + '/Handler/PacketHandler.cs', 'w+')
	f.write(output)
	f.close()

	if args.isPrint:
		print(output)

	print( "\033[32m----------Start Client Send Packet----------\033[0m" )

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

		if args.isPrint:
			print(eachHandler)
		

	print( "\033[32m----------End Client Send Packet----------\033[0m" )

####################################################################################################

	return


if __name__ == '__main__':
	main()