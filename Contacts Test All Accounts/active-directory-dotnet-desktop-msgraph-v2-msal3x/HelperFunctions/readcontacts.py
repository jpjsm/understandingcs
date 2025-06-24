import json

with open('C:\\tmp\\contacts.json', 'r', encoding='utf-8') as infile:
    contacts = json.load(infile)
with open('C:\\tmp\\contacts-beautified.json', 'w', encoding='utf-8') as outfile:
    json.dump(contacts, outfile, sort_keys=True, indent=4)

print("Total contacts: {0}".format(len(contacts)))
keys = {}
for contact in contacts:
    for k in contact:
        if not k in keys:
            keys[k] = []

        if contact[k]:
            keys[k] += [contact[k]]

for key in keys:
    print(key, len(keys[key]))
    with open("C:\\tmp\\" + str(key) + ".json", 'w', encoding='utf-8') as outfile:
            json.dump(keys[key], outfile, sort_keys=True, indent=4)
