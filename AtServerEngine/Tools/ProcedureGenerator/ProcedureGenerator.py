import argparse
import jinja2
import XmlDBParser

def main():
    arg_parser = argparse.ArgumentParser(description = 'StoredProcedure Generator')
    arg_parser.add_argument('--path', type=str, default='C:/Users/ath331/Desktop/a/Game/T_Survivor/AtServerEngine/TitleServer/DB/TitleDB.xml', help='Xml Path')
    arg_parser.add_argument('--dbName', type=str, default='atserver_title', help='db name')
    arg_parser.add_argument('--output', type=str, default='GenProcedures.h', help='Output File')
    arg_parser.add_argument('--isPrint', type=bool, default=True, help='Is Print')
    args = arg_parser.parse_args()

    if args.path == None or args.output == None:
        print('[Error] --path --output required')
        return

    parser = XmlDBParser.XmlDBParser()
    parser.parse_xml(args.path)

    file_loader = jinja2.FileSystemLoader('Templates')
    env = jinja2.Environment(loader=file_loader)
    template = env.get_template('GenProcedures.h')

    output = template.render(dbName=args.dbName, procs=parser.procedures)
    f = open(args.output, 'w+')
    f.write(output)
    f.close()

    if args.isPrint:
        print(output)

if __name__ == '__main__':
    main()