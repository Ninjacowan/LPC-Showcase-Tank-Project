import pymel.core as pm
import maya.cmds as cmds

def CreateJoints(): 
 selected = cmds.ls(sl=True)
 for s in selected:
     pos = cmds.xform(s, q=True, t=True, ws=True)
     cmds.select(clear=True) 
     cmds.joint( p=pos[0:3],name=s+'Joint')
     cmds.pointConstraint( s, s+'Joint' )      

def UI():
    if pm.window('RJ_Windows', ex=1):
        pm.deleteUI('RJ_Windows')

    pm.window('RJ_Windows', h=145, s=True, t="Rivet Joints V1.0", w=345)
    pm.columnLayout('RJ_Windows', adj=True)
    pm.separator(h=25, style="none")

    
    pm.rowColumnLayout(cs=[(1, 10)],numberOfColumns=1,bgc=[0.2, 0.2, 0.2])
    pm.text( l="Please select the Locators...")
    pm.setParent("..")
    
    pm.separator(h=25, style="none")
    pm.separator(h=5, style="none")
    pm.rowColumnLayout(cs=[(1, 10)], numberOfColumns=2)
    pm.button(c=lambda *args: CreateJoints() , bgc=(0.314, 0.70, 0.70), align="center", height=30,width=300, label="Create Joints")
    pm.setParent("..")
    pm.separator(h=25, style="none")
    pm.setParent('..')
    pm.showWindow('RJ_Windows')
    
UI()